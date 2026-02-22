import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    reporters: ['dot'],
    isolate: false,
    coverage: {
      reporter: ['text', 'cobertura', 'html']
    },
    test: {
      name: { label: 'tests', color: 'purple' },
      include: ['./src/**/*.spec.mjs']
    }
  }
});
