const PROXY_CONFIG = [
    {
        context: ["/api","/hubs"],
        target: "https://localhost:7157",
        secure: false,
    },
];

module.exports = PROXY_CONFIG;
