import axios from 'axios';

export default {

  login(user) {
    return axios.post('/admin/login', user)
  },

  register(user) {
    return axios.post('/admin/register', user)
  }

}
