import Prism from 'prismjs';
import 'prismjs/plugins/autoloader/prism-autoloader.min.js';
import 'prismjs/plugins/custom-class/prism-custom-class.min.js';

Prism.plugins.autoloader.languages_path = 'grammars/';
Prism.plugins.customClass.prefix('prism--');

export default () => Prism.highlightAll();