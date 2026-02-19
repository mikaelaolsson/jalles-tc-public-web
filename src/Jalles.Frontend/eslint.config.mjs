import js from '@eslint/js';
import globals from 'globals';
import jsdoc from 'eslint-plugin-jsdoc';
import stylistic from '@stylistic/eslint-plugin';

/** @type {import('eslint').Linter.FlatConfig} */
const ignorePatterns = {
  ignores: ['node_modules/', '**/.*', '.eslintcache'],
};

/** @type {import('eslint').Linter.FlatConfig} */
const baseConfig = {
  files: ['scripts/*.{js,mjs}', '*.{js,mjs}'],
  languageOptions: {
    ecmaVersion: 'latest',
    sourceType: 'module',
    globals: {
      ...globals.browser
    }
  },
  plugins: {
    jsdoc,
    '@stylistic/js': stylistic
  },
  rules: {
    ...js.configs.recommended.rules,
    ...jsdoc.configs.recommended.rules,
    'jsdoc/require-jsdoc': 'off',
    'require-jsdoc': 'off',
    'eqeqeq': 'error',
    'no-extra-boolean-cast': 'off',
    'no-var': 'error',
    '@stylistic/js/indent': ['error', 2, { 'SwitchCase': 1 }],
    '@stylistic/js/quotes': ['error', 'single', { 'allowTemplateLiterals': 'always' }],
    '@stylistic/js/semi': ['error', 'always'],
    '@stylistic/js/no-trailing-spaces': 'error',
    '@stylistic/js/no-multi-spaces': 'error',
    '@stylistic/js/space-infix-ops': 'error',
    '@stylistic/js/space-before-function-paren': ['error', 'never'],
    '@stylistic/js/space-in-parens': ['error', 'never'],
    '@stylistic/js/keyword-spacing' : ['error', { 'before': true, 'after': true }],
  }
};

export default [ignorePatterns, baseConfig];
