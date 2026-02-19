/* global __dirname */
import { resolve } from 'node:path';
import { defineConfig } from 'vite';
import mkcert from 'vite-plugin-mkcert';
import browserslist from 'browserslist';
import { resolveToEsbuildTarget } from 'esbuild-plugin-browserslist';
import copy from 'rollup-plugin-copy';


const esBuildTarget = resolveToEsbuildTarget(browserslist(), {
  printUnknownTargets: false
});

export default defineConfig({
  plugins: [
    mkcert(),
    copy({
      hook: 'buildStart',
      targets: [
        { src: 'static/**/*', dest: resolve(__dirname, '../Jalles.Web/wwwroot/static') }
      ]
    })
  ],
  build: {
    target: esBuildTarget,
    outDir: resolve(__dirname, '../Jalles.Web/wwwroot/'),
    emptyOutDir: false,
    assetsDir: 'assets',
    manifest: true,
    manifestDir: '.vite',
    sourcemap: true,
    rollupOptions: {
      input: resolve(__dirname, 'scripts/index.mjs'),
      plugins: []
    }
  },
  server: {
    port: 5010,
    origin: `https://127.0.0.1:5010`,
    https: true,
    strictPort: true
  }
});
