import Prism from 'prismjs';
import 'prismjs/plugins/autoloader/prism-autoloader.min.js';
import 'prismjs/plugins/custom-class/prism-custom-class.min.js';

Prism.plugins.autoloader.languages_path = 'https://jonathanjameswatson.com/markdraw/grammars/';
Prism.plugins.customClass.prefix('prism--');

export const highlightHtml = (code) => Prism.highlight(code, Prism.languages.html, 'html');

export default () => Prism.highlightAll();