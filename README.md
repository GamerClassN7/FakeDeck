<div align="center">
    <img style="border-radius:10px" src="images/logo.png" title="Logo"><br>
    <img src="https://img.shields.io/github/downloads/GamerClassN7/FakeDeck/total.svg" alt="Github All Releases">
</div>

# FakeDeck - Poor Man Macro Pad

Creates simple webserver with buttons whitch can be opened on any device an can be used as macro pad/keyboard

![alt text](images/image.png)

## How to use it ?

Just add desired macros to `configuration.yaml` and start the application, tahn zou can load dashboard on any web capable device inside of your network.

## Example Macros:
### Helldivers 2 Macros
```yaml
- button: reinforce
  function: HelldiversTwoMacro
  parameters:
    - name: Key
      value: reinforce
```
```yaml
- button: strafing-run
  function: HelldiversTwoMacro
  parameters:
    - name: Key
      value: strafing-run
```
```yaml
- button: airstrike
  function: HelldiversTwoMacro
  parameters:
    - name: Key
      value: airstrike
```
```yaml
- button: cluster-bomb
  function: HelldiversTwoMacro
  parameters:
    - name: Key
      value: cluster-bomb
```
```yaml
- button: napalm-airstrike
  function: HelldiversTwoMacro
  parameters:
    - name: Key
      value: napalm-airstrike
```
```yaml
- button: smoke-strike
  function: HelldiversTwoMacro
  parameters:
    - name: Key
      value: smoke-strike
```
```yaml
- button: rocket-pods
  function: HelldiversTwoMacro
  parameters:
    - name: Key
      value: rocket-pods
```
```yaml
- button: bomb
  function: HelldiversTwoMacro
  parameters:
    - name: Key
      value: bomb
```
### Media Control Macros
```yaml
- button: mute
  function: MediaMacro
  parameters:
    - name: Key
      value: mute
```
```yaml
- button: previous
  function: MediaMacro
  parameters:
    - name: Key
      value: previous
```
```yaml
- button: play
  function: MediaMacro
  parameters:
    - name: Key
      value: play/pause
```
```yaml
- button: next
  function: MediaMacro
  parameters:
    - name: Key
      value: next
```
### FakeDeck UI Control Macros
```yaml
- button: full-screen
  function: FakeDeckMacro
  parameters:
    - name: Key
      value: full-screen
```
```yaml
- button: set-page
  function: FakeDeckMacro
  parameters:
    - name: Key
      value: set-page
    - name: PageId
      value: {{page-id}}
```
```yaml
- button: spacer
  function: FakeDeckMacro
  parameters:
    - name: Key
      value: spacer
```
### Process Macro
```yaml
- button: cmd
  function: ProcessMacro
  parameters:
    - name: process
      value: "cmd.exe"
    - name: arguments #Optional Proces Arguments
      value: "-d C:/"
```
### Comon Macro Parameters
```yaml
...
parameters:
- name: Color
  value: White #CSS friendly color
- name: Image
  value: White #URL To Image (Local Path is not supported now)
...
```
## Contributors
<a href="https://github.com/GamerClassN7/FakeDeck/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=GamerClassN7/FakeDeck" />
</a>

## Star History
[![Star History Chart](https://api.star-history.com/svg?repos=GamerClassN7/FakeDeck&type=Timeline)](https://star-history.com/#GamerClassN7/FakeDeck&Timeline)]