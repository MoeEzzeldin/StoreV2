/**
 * ImageService.js
 * A utility service for handling image-related operations
 * Provides a consistent dark mode placeholder for product images
 */

export default {
  /**
   * Returns a dark mode compatible default image for all products
   * 
   * @param {Object} product - The product object (unused but kept for compatibility)
   * @returns {string} - The default dark mode image URL
   */
  generateProductImage(product) {
    // If the product already has a valid picture URL (not a placeholder), return it
    if (product.pictureUrl && !product.pictureUrl.includes('placeholder')) {
      return product.pictureUrl;
    }
    
    // Return a dark-mode friendly placeholder image
    // Using a data URI for a simple dark gray square with product icon
    return 'data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyMDAiIGhlaWdodD0iMjAwIiB2aWV3Qm94PSIwIDAgMjAwIDIwMCI+PHJlY3Qgd2lkdGg9IjIwMCIgaGVpZ2h0PSIyMDAiIGZpbGw9IiMyMjIiLz48cGF0aCBkPSJNMTMwIDY1SDcwYy01LjUgMC0xMCA0LjUtMTAgMTB2NTBjMCA1LjUgNC41IDEwIDEwIDEwaDYwYzUuNSAwIDEwLTQuNSAxMC0xMFY3NWMwLTUuNS00LjUtMTAtMTAtMTB6bS0yMCA3MGMwIDEuMS0uOSAyLTIgMkg5MmMtMS4xIDAtMi0uOS0yLTJWNzZjMC0xLjEuOS0yIDItMmgxNmMxLjEgMCAyIC45IDIgMnY1OXptMjAtMjJjMCAxLjEtLjkgMi0yIDJoLTRjLTEuMSAwLTItLjktMi0yVjc2YzAtMS4xLjktMiAyLTJoNGMxLjEgMCAyIC45IDIgMnYzN3oiIGZpbGw9IiM0NDQiLz48L3N2Zz4=';
  }
};
