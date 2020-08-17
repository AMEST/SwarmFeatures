# Simple proxy server for docker swarm   
**[DockerHub](https://hub.docker.com/r/eluki/swarm-auto-proxy)**

This proxy server automatically finds services to which traffic should be redirected.   
For example, the service is published on port 9000, but each time you have to remember to specify the port in the address. But using this proxy, you can specify three labels at the service: 
1. "sf.proxy.enable=true"
1. "sf.proxy.hostname=peresh.tk"
1. "sf.proxy.address=serverip:9000" 
Thereby, traffic coming to the server with the specified domain name will be redirected to the container.

## Using
### Proxy stack file
```yaml
version: '3.7'

services:
  autoproxy:
    image: eluki/swarm-auto-proxy:latest
    environment:
      - "DockerClient:Uri=unix:///var/run/docker.sock"
    ports:
      - 80:80
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock
    deploy:
      placement:
        constraints:
          - node.role == manager
```

### Example stack for exposed service:
```yaml
version: '3.7'

services:
  web:
    image: eluki/peresh:latest
    ports:
     - 30001:80
    deploy:
      labels:   # defining a container label instead of an image label
        - sf.proxy.enable=true
        - sf.proxy.hostname=peresh.tk
        - sf.proxy.address=10.8.0.1:30001
```
