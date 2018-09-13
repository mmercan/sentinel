importScripts('sw-toolbox.js');
const openInanewWindow = false;
const spCaches = {
  'static': 'static-v5',
  'dynamic': 'dynamic-v5'
}

self.addEventListener("install", function (event) {
  event.waitUntil(
    caches.open(spCaches.static)
    .then(function (cache) {
      return cache.addAll([
        "/assets/offline.html",
        "/assets/images/logos/angular.png",
        "/assets/fonts/font-awesome-4.7.0/fonts/fontawesome-webfont.woff2",
        "/assets/fonts/font-awesome-4.7.0/fonts/fontawesome-webfont.woff",
        "/assets/fonts/font-awesome-4.7.0/fonts/fontawesome-webfont.eot",
        "/assets/fonts/font-awesome-4.7.0/fonts/fontawesome-webfont.woff2?v=4.7.0",
        "/assets/fonts/font-awesome-4.7.0/fonts/fontawesome-webfont.woff?v=4.7.0",
        "/assets/fonts/font-awesome-4.7.0/fonts/fontawesome-webfont.eot?v=4.7.0"
      ]);
      console.log("Service Worker %s Installed and Cached !!!! ", spCaches.static, new Date().toLocaleTimeString())
    })
  );
});

self.addEventListener("activate", function (event) {
  event.waitUntil(
    caches.keys()
    .then(function (keys) {
      return Promise.all(keys.filter(function (key) {
        return !Object.values(spCaches).includes(key);
      }).map(function (key) {
        return caches.delete(key);
      }));
      console.log("Service Worker %s Activated with Cache Clear ", spCaches.static, new Date().toLocaleTimeString())
    })
  );
});

self.addEventListener('push', event => {
  var payload = event.data.json();
  console.log(payload);
  var title = "New Message";
  if (payload.title) {
    title = payload.title;
  }
  options = {
    body: payload.Message,
    icon: './assets/icons/android-chrome-192x192.png',
    badge: './assets/icons/android-chrome-192x192.png',
    data: payload,
    actions: [{
      action: 'view',
      title: 'See it',
      icon: './assets/icons/android-chrome-512x512.png'
    }, {
      action: 'later',
      title: 'Pop it later',
      icon: './assets/icons/android-chrome-512x512.png'
    }]
  };
  event.waitUntil(self.registration.showNotification(title, options));
});

self.addEventListener('notificationclick', event => {
  event.notification.close();
  var payload = event.notification.data;
  console.log(event);
  console.log("-------------");
  //console.log(event.data.url);
  var location = event.target.location.origin + '/';
  var notificationpath = "#/notification";
  switch (event.action) {
    case 'later':
    case 'view':
    default:
      //event.waitUntil(clients.openWindow(location + notificationpath));
      var found = false;
      var cc = clients;
      var promise = clients.matchAll({
        includeUncontrolled: true,
        type: 'window'
      }).then(function (clients) {
        for (i = 0; i < clients.length; i++) {
          var client = clients[i];
          //if (client.url === '/' && 'focus' in client) {
          if ('focus' in client && !openInanewWindow) {
            found = true;
            //client.url = location + notificationpath;
            client.navigate(location + notificationpath);
            return client.focus();
            break;
          }
        }
        if (!found) {
          // event.waitUntil(cc.openWindow(location + notificationpath));
          cc.openWindow(location + notificationpath).then(function (windowClient) {
            //  // do something with the windowClient.
            console.log("openning");
          });
        }
      });
      event.waitUntil(promise);
  }
});

//toolbox.precache(['/index.html',"/assets/offline.html","/sw.js","/assets/fonts/font-awesome-4.7.0/fonts/fontawesome-webfont.woff2","/assets/fonts/font-awesome-4.7.0/fonts/fontawesome-webfont.woff","/assets/fonts/font-awesome-4.7.0/fonts/fontawesome-webfont.eot"]);

// toolbox.router.get('/(.*)', toolbox.networkFirst, {
//   networkTimeoutSeconds: 5
// });

toolbox.router.get('/*', function (request, values, options) {
  //   return toolbox.fastest(request, values, options)
    return toolbox.networkFirst(request, values, options)
    .catch(function (err) {
      return caches.match(new Request("/assets/offline.html"));
    });
}, {
  cache: {
    networkTimeoutSeconds: 5,
    name: spCaches.static,
    maxageSeconds: 60 * 60 * 24
  }
});


// toolbox.router.get('/assets/(.*)', function (request, values, options) {
//   return toolbox.networkFirst(request, values, options)
//     .catch(function (err) {
//       return caches.match(new Request("/assets/offline.html"));
//     });
// }, {
//   cache: {
//     networkTimeoutSeconds: 5,
//     name: spCaches.static,
//     maxageSeconds: 60 * 60 * 24
//   }
// });

// toolbox.router.get('/*.css',function(request,values,options){
//     return toolbox.networkFirst(request,values,options)
//     .catch(function(err){
//         return caches.match(new Request("/assets/offline.html"));
//     });
// }, {
//     cache:{
//         networkTimeoutSeconds:10,
//         name:spCaches.static,
//         maxageSeconds : 60 *60 * 24
//     }
// });

// self.addEventListener("fetch",function(event){       
//         console.log("fetch called : " + event.request.url);
//            event.respondWith(
//             networkFirst(event.request)
//             //cacheFirst(event.request)
//         );
//         //   if(!navigator.onLine){event.respondWith(new Response('<h1> Offline :( </h1>',{headers:{'Content-Type':'text/html'}}));  }else{ event.respondWith(fetch(event.request)); }
// });

function fetchandUpdate(request) {
  return fetch(request)
    .then(function (res) {
      if (res) return caches.open(spCaches.static)
        .then(function (cache) {
          return cache.put(request, res.clone())
            .then(function () {
              return res;
            })
        })
    })
}

function networkFirst(request) {
  return fetch(request)
    .then(function (fResponse) {
      return caches.open(spCaches.static).then(function (cache) {
        if (!fResponse.ok) {
          return cache.match(request)
        } else {
          cache.put(request, fResponse.clone());
          return fResponse;
        }
      })
    }).catch(function (error) {
      console.error(error);
      return caches.open(spCaches.static).then(function (cache) {
        return cache.match(request)
      });
    })
};

function cacheFirst(request) {
  return caches.match(request).then(function (cResponse) {
    if (cResponse) {
      return cResponse;
    }
    return fetch(request).then(function (fResponse) {
      return caches.open(spCaches.static).then(function (cache) {
        return cache.put(request, fResponse.clone()).then(function () {
          return fResponse;
        })
      })
    })
  })
}


