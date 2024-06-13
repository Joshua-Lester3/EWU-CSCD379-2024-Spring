import Axios from 'axios';

export default defineNuxtPlugin(() => {
  if (process.client) {
    if (
      window.location.hostname === 'localhost' ||
      window.location.hostname === '172.31.112.1'
    ) {
      Axios.defaults.baseURL = 'https://localhost:7166/';
    } else {
      Axios.defaults.baseURL = 'https://rhymeapi.azurewebsites.net/';
    }
  }
});
