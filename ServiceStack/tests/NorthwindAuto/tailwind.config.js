module.exports = {
  content: [
    './ui/**/*.{mjs,html}',
    './admin-ui/**/*.{html,mjs}',
    './locode/**/*.{html,mjs}', 
    './shared/**/*.html', 
    './custom/**/*.html',
    './wwwroot/**/*.{html,mjs}',
    './locode-v1/**/*.html',
    './ui-v1/**/*.html',
    '../../src/ServiceStack/js/servicestack-vue.mjs'
  ],
  theme: {
    extend: {},
  },
  plugins: [
    require('@tailwindcss/forms')
  ],
}
