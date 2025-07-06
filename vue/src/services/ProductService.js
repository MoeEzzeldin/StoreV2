import axios from 'axios';

export default {
  list() {
    return axios.get('/product');
  },
  getProduct(id) {
    return axios.get(`/product/${id}`);
  },
  search(productName) {
    return axios.get(`/product?productName=${productName}`);
  },

  productFilterOne() {
    return axios.get('/product/price', {
      params: {
        min: 0,
        max: 200,
      },
    });
  },

  productFilterTwo() {
    return axios.get('/product/price', {
      params: {
        min: 201,
        max: 500,
      },
    });
  },

  productFilterThree() {
    return axios.get('/product/price', {
      params: {
        min: 500,
        max: 10000,
      },
    });
  },
};
