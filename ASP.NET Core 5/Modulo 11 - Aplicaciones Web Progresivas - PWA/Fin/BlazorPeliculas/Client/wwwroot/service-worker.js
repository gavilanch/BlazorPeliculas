self.addEventListener('push', event => {
    const payload = event.data.json();

    event.waitUntil(
        self.registration.showNotification('Nueva Película en Cines', {
            body: payload.titulo,
            image: payload.imagen,
            data: {url: payload.url}
        })
    );
});

self.addEventListener('notificationclick', event => {
    event.notification.close();
    event.waitUntil(clients.openWindow(event.notification.data.url));
});