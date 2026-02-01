# SSH Settings
- As part of the automated pipeline, allowing Github servers to SSH into the remote VM to make changes is necessary.
- The question that came to mind was "which IP range am I allowing to access the SSH port (port 22)?" since up until now, only my local machine's IP had been allowed.
- The answer is all of them.
- This sounds unsafe and it is without proper measures.
- Therefore, the measures taken to secure the VM against SSH brute force attacks are:
  - Upping password strength
  - Using SSH keys for authentication
  - Installing [Fail2Ban](https://github.com/fail2ban/fail2ban) using this [guide](https://www.digitalocean.com/community/tutorials/how-to-protect-ssh-with-fail2ban-on-ubuntu-20-04) to create long timeouts whenever a bot tries to brute force attack my VM.
    - The guide fails when starting the service as I got a `Failed to enable unit: Unit file fail2ban.service does not exist.` so just follow the readme to copy over the system init/service script (in this case `files/debian-initd`).
- Another option considered was using [Watchtower](https://github.com/containrrr/watchtower), but it clearly states on the README that it's not intended for production apps.
  
# Debugging
- If you're having trouble with files:
- Use `ENTRYPOINT ["tail", "-f", "/dev/null"]` right before the problem command and comment the following commands out.
- This keeps the container running allowing for browsing of files.
## No Main() in .Net generated dockerfile
- See https://medium.com/@jakubrzepka/building-an-asp-net-8-web-api-docker-image-container-8a64f8635275