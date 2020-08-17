# Simple service scheduler for periodic launch containers   
**[DockerHub](https://hub.docker.com/r/eluki/swarm-scheduler)**

This Scheduler automatic find services with special labels, read cron and add service to schedule. Scheduler run containers in service by cron rule.   
For using scheduler, add labels to service: 
1. "sf.scheduler.enable=true" - enable service for schedule
1. "sf.scheduler.cron=0 * * * * ? *" - cron rule
1. "sf.scheduler.replicas=2" - container replicas count.

## Using
### Scheduler stack file
```yaml
version: '3.7'
          
  scheduler:
    image: eluki/swarm-scheduler:latest
    environment:
      - "DockerClient:Uri=unix:///var/run/docker.sock"
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock
    deploy:
      placement:
        constraints:
          - node.role == manager

```

### Example stack for exposed service:
```yaml
version: "3.7"

services:
  test:
    image: busybox
    command: date
    deploy:
      labels:
        #- "swarm.cronjob.enable=true"
        #- "swarm.cronjob.schedule=0 * * * * *"
        #- "swarm.cronjob.skip-running=false"
        - "sf.scheduler.enable=true"
        - "sf.scheduler.cron=*/10 * * * * ? *"
      replicas: 0
      restart_policy:
        condition: none
```
