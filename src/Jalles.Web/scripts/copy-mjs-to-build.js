// Simple Node.js script to copy all .mjs files from scripts/ to wwwroot/build/scripts/
// TODO: remove when we have a proper frontend project
const fs = require('fs');
const path = require('path');

const srcDir = path.join(__dirname);
const destDir = path.join(__dirname, '../wwwroot/build/scripts');

if (!fs.existsSync(destDir)) {
  fs.mkdirSync(destDir, { recursive: true });
}

const files = fs.readdirSync(srcDir);
files.forEach(file => {
  if (file.endsWith('.mjs')) {
    fs.copyFileSync(path.join(srcDir, file), path.join(destDir, file.replace('.mjs', '.js')));
    console.log(`Copied ${file} to build/scripts as ${file.replace('.mjs', '.js')}`);
  }
});
