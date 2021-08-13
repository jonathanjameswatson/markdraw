const fs = require('fs');
const yaml = require('js-yaml');
const toc = yaml.load(fs.readFileSync('doc/api/toc.yml'));

const namespaces = {};

for (let i = 0; i < toc.length; i++) {
    const fullNamespace = toc[i].uid;
    const splitNamespace = fullNamespace.split('.');

    let parent = namespaces;

    for (let j = 0; j < splitNamespace.length; j++) {
        const partialNamespace = splitNamespace[j];

        if (parent[partialNamespace] === undefined) {
            parent[partialNamespace] = {};
        }
        parent = parent[partialNamespace];
    }

    if (parent.items === undefined) {
        parent.items = toc[i].items;
    } else {
        parent.items.push(toc[i]);
    }
}

function recurse(obj, path = "") {
    const items = [];
    Object.keys(obj).forEach((e, i) => {
        if (e !== "items") {
            let newPath;
            if (path === "") {
                newPath = e;
            } else {
                newPath = path + '.' + e;
            }
            const newObj = {uid: newPath, name: newPath, items: obj[e].items || []};
            newObj.items.push(...recurse(obj[e], newPath));
            items.push(newObj);
        }
    });
    return items;
}

const items = recurse(namespaces)[0];

fs.writeFileSync('doc/api/toc.yml', yaml.dump(items));