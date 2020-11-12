var redis = require("redis");
var client = redis.createClient();

client.on("connect", function () {
    console.log("You are now connected");
});


client.set("student", "Laylaa", function (err, reply) {
    console.log(reply);
});