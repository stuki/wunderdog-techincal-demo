module.exports = {
  purge: {
    content: ['./components/**/*.tsx', './pages/**/*.tsx'],
  },
  theme: {
    extend: {
      boxShadow: {
        '2xl': '0 0 50px 2px #71A6481A',
      },
      height: {
        iphone: '500px',
      },
      minWidth: {
        iphone: '250px',
      },
    },
  },
  variants: {
    extend: {
      opacity: ['disabled'],
    },
  },
  plugins: [require('tailwindcss-font-inter')()],
};
