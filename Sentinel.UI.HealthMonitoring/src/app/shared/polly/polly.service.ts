import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PollyService {

  defaults = {
    delay: 100,
  };
  constructor() { }

  public execute(config, cb) {
    let count = 0;

    while (true) {
      try {
        return cb({ count: count });
      } catch (ex) {
        if (count < config.count && config.handleFn(ex)) {
          count++;
        } else {
          throw ex;
        }
      }
    }
  }

  public executeForPromise(config, cb) {
    let count = 0;
    return new Promise((resolve, reject) => {
      function execute() {
        const original = cb({ count: count });

        original.then((e) => {
          resolve(e);
        }, (e) => {
          if (count < config.count && config.handleFn(e)) {
            count++;
            execute();
          } else {
            reject(e);
          }
        });
      }
      execute();
    });
  }

  public executeForPromiseWithDelay(config, cb) {
    let count = 0;

    return new Promise((resolve, reject) => {
      function execute() {
        const original = cb({ count: count });

        original.then((e) => {
          resolve(e);
        }, (e) => {
          const delay = config.delays.shift();

          if (delay && config.handleFn(e)) {
            count++;
            setTimeout(execute, delay);
          } else {
            reject(e);
          }
        });
      }
      execute();
    });
  }

  public executeForNode(config, fn, callback) {
    let count = 0;

    function internalCallback(err, data) {
      if (err && count < config.count && config.handleFn(err)) {
        count++;
        fn(internalCallback, { count: count });
      } else {
        callback(err, data);

      }
    }

    fn(internalCallback, { count: count });
  }

  public executeForNodeWithDelay(config, fn, callback) {
    let count = 0;

    function internalCallback(err, data) {
      const delay = config.delays.shift();
      if (err && delay && config.handleFn(err)) {
        count++;
        setTimeout(() => {
          fn(internalCallback, { count: count });
        }, delay);
      } else {
        callback(err, data);
      }
    }

    fn(internalCallback, { count: count });
  }

  public delayCountToDelays(count) {
    const delays = [];
    let delay = this.defaults.delay;

    for (let i = 0; i < count; i++) {
      delays.push(delay);
      delay = 2 * delay;
    }

    return delays;
  }

  pollyFn = () => {
    const config = {
      count: 1,
      delays: [this.defaults.delay],
      handleFn: () => {
        return true;
      },
    };

    return {
      handle: (handleFn) => {
        if (typeof handleFn === 'function') {
          config.handleFn = handleFn;
        }
        return this;
      },
      retry: (count) => {
        if (typeof count === 'number') {
          config.count = count;
        }

        return {
          execute: this.execute.bind(null, config),
          executeForPromise: this.executeForPromise.bind(null, config),
          executeForNode: this.executeForNode.bind(null, config),
        };
      },
      waitAndRetry: (delays) => {
        if (typeof delays === 'number') {
          delays = this.delayCountToDelays(delays);
        }

        if (Array.isArray(delays)) {
          config.delays = delays;
        }

        return {
          executeForPromise: this.executeForPromiseWithDelay.bind(null, config),
          executeForNode: this.executeForNodeWithDelay.bind(null, config),
        };
      },
    };

  }

}
