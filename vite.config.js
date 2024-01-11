import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  root: "./src",
  base: "https://www.slater.dev/fpace",
  build: {
    outDir: "../dist",
  }
})
