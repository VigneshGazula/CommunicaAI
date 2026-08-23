const LOCAL_BACKEND = 'http://localhost:5169';
const PROD_BACKEND = 'https://communicaai.onrender.com';

export const environment = {
  production: true,
  apiBaseUrl:
    typeof window !== 'undefined' &&
    (window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1')
      ? LOCAL_BACKEND
      : PROD_BACKEND
};
