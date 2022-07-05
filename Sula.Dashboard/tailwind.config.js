module.exports = {
  purge: {
    content: ['./components/**/*.tsx', './pages/**/*.tsx'],
  },
  theme: {
    extend: {
      gridTemplateColumns: {
        content: '1fr 7fr',
      },
      gridTemplateRows: {
        layout: '4rem 3rem 1fr',
        layoutMobile: '2rem 2rem 1fr',
      },
      height: {
        sm: '12rem',
        md: '24rem',
        lg: '32rem',
        xl: '42rem',
      },
      minHeight: {
        fullIsh: '85%',
      },
    },
  },
};
