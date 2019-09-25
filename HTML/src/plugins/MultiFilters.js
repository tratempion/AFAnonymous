class MultiFilters {

    constructor(items, filters, filter, headers) {

      this.items = items;
      this.filter = filter;
      this.headers = headers;
      this.filters = filters;
      this.filterCallbacks = {};
    }

    static updateFilters(filters, val) {

      return Object.assign({}, filters, val);
    }
  
    registerFilter(filterName, filterCallback) {

      this.filterCallbacks[filterName] = filterCallback;
    }
  
    runFilters() {

      const context = this;

      let filteredItems = context.items;
  
      Object.entries(this.filterCallbacks).forEach(([entity, cb]) => {
          
          filteredItems = cb.call(context, context.filters[entity], filteredItems);
        });
  
      return filteredItems;
    }
  
  }
  
  const MultiFiltersPlugin = {

    install(Vue, options) {

      Vue.prototype.$MultiFilters = MultiFilters;
    }
  };
  
  export default MultiFiltersPlugin;