import axios from 'axios';

export default {

  login(user) {
    return axios.post('auth/login', user)
  },

  register(user) {
    return axios.post('auth/register', user)
  }

}
