import Prism from 'prismjs';

Prism.manual = true;
Prism.plugins.autoloader.languages_path = 'https://jonathanjameswatson.com/markdraw/grammars/';
Prism.plugins.customClass.prefix('prism--');

export const highlightHtml = (code) => Prism.highlight(code, Prism.languages.html, 'html');

export default () => Prism.highlightAll();
