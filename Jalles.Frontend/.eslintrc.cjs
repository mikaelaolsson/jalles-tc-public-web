module.exports = {
	root: true,
	parser: '@typescript-eslint/parser',
	extends: ['eslint:recommended', 'plugin:@typescript-eslint/recommended'],
	plugins: ['@typescript-eslint'],
	ignorePatterns: ['*.cjs', '*.scss'],
	parserOptions: {
		sourceType: 'module',
		ecmaVersion: 2015
	},
	env: {
		es2020: true,
		node: true
	},
  rules: {
    "no-trailing-spaces": "error",
    "no-multi-spaces": "error",
    "no-multiple-empty-lines": ["error", { "max": 1, "maxEOF": 0, "maxBOF": 2 }],
    "space-infix-ops": "error",
    "brace-style": ["error", "1tbs", { "allowSingleLine": true }],
    "comma-dangle": "error",
    "space-before-function-paren": ["error", {
      "anonymous": "never",
      "named": "never",
      "asyncArrow": "always"
    }],
    "space-in-parens": ["error", "never"],
    "arrow-spacing": ["error", { "before": true, "after": true }],
    "eqeqeq": "error",
    "quotes": ["error", "single", { "allowTemplateLiterals": true }],
    "@typescript-eslint/explicit-function-return-type": ["error"],
    "indent": "off",
    "@typescript-eslint/indent": ["error", 2, { "SwitchCase": 1 }],
    "keyword-spacing": "off",
    "@typescript-eslint/keyword-spacing": ["error", {
      "after": true,
      "overrides": {
        "if": { "after": false },
        "for": { "after": false },
        "catch": { "after": false },
        "while": { "after": false }
      }
    }],
    "no-explicit-any": "off",
    "@typescript-eslint/no-explicit-any": "error",
    "no-unused-vars": "off",
    "@typescript-eslint/no-unused-vars": ["error"],
    "semi": "off",
    "@typescript-eslint/semi": ["error"],
    "@typescript-eslint/no-non-null-assertion": "off",
    "@typescript-eslint/ban-ts-comment": "off",
  },
};
