// @ts-check
const eslint = require("@eslint/js");
const tseslint = require("typescript-eslint");
const angular = require("angular-eslint");
const importPlugin = require("eslint-plugin-import");
const unusedImports = require("eslint-plugin-unused-imports");

module.exports = tseslint.config(
  {
    files: ["**/*.ts"],
    plugins: {
      import: importPlugin,
      "unused-imports": unusedImports,
    },
    extends: [
      eslint.configs.recommended,
      ...tseslint.configs.recommended,
      ...tseslint.configs.stylistic,
      ...angular.configs.tsRecommended,
    ],
    processor: angular.processInlineTemplates,
    rules: {
      "@angular-eslint/directive-selector": [
        "error",
        {
          type: "attribute",
          prefix: "ton",
          style: "camelCase",
        },
      ],
      "@angular-eslint/component-selector": [
        "error",
        {
          type: "element",
          prefix: "ton",
          style: "kebab-case",
        },
      ],
      "import/order": [
        "error",
        {
          groups: [
            ["builtin", "external"],
            "internal",
            ["index", "sibling", "parent"],
          ],
          "newlines-between": "always",
          alphabetize: {
            order: "asc",
            caseInsensitive: true,
          },
          pathGroups: [
            {
              pattern: "@components/**",
              group: "internal",
              position: "before",
            },
            {
              pattern: "@{app-types,api}/**",
              group: "internal",
              position: "before",
            },
          ],
          pathGroupsExcludedImportTypes: ["builtin"],
        },
      ],
      "import/no-absolute-path": ["error"],
      "import/newline-after-import": ["error", { count: 1 }],
      "@typescript-eslint/explicit-member-accessibility": [
        "error",
        { overrides: { constructors: "off" } },
      ],
      "@typescript-eslint/array-type": ["error", { default: "array-simple" }],
      "@typescript-eslint/no-duplicate-enum-values": "error",
      "@typescript-eslint/prefer-as-const": "error",
      "@typescript-eslint/member-ordering": [
        "error",
        {
          default: [
            "public-static-field",
            "public-instance-field",
            "private-static-field",
            "private-instance-field",
            "public-constructor",
            "private-constructor",
            "public-instance-method",
            "protected-instance-method",
            "private-instance-method",
          ],
        },
      ],
      "no-multiple-empty-lines": "error",
      quotes: ["error", "single", { allowTemplateLiterals: true }],
      camelcase: ["error"],
      "@typescript-eslint/no-unused-vars": "off",
      "unused-imports/no-unused-imports": "off",
      "no-multi-spaces": ["error"],
      "lines-between-class-members": ["error"],
      "no-var": ["error"],
    },
  },
  {
    files: ["**/*.html"],
    extends: [
      ...angular.configs.templateRecommended,
      ...angular.configs.templateAccessibility,
    ],
    rules: {},
  }
);
