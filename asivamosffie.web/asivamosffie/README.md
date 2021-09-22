# Asivamosffie

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 9.1.7.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).


## para crear una vista usar el comando

    ng g m _pages/<module-name> --routing

y generamos el componente (en angular material hay algunos que podemos utilizae [Angular Material](https://material.angular.io/guide/schematics).

    ng generate @angular/material:address-form _pages/<module-name>/components/<component-name>

modal dialog

    ng g c _pages/<module-name>/components/dialog-<component-name> --skipTests
luego en el modulo de pone
    `entryComponents: [dialog-<component-name],`

## Si tiene problemas a la hora de correr el proyecto localmente

    npm run compilar

## Compilar un servidor local por wifi

en el cmd
    ipconfig

con la ip local
    node --max-old-space-size=8192 ./node_modules/@angular/cli/bin/ng serve --port 4200 --host="ip local" --disable-host-check

## Desplegar en [asivamos.ffie.com.co](https://asivamos.ffie.com.co)

cuenta cambiar en proxy.conf.json
    "target":"https://asivamosback.ffie.com.co",

luego utilice el comando
    npm run prod
## Desplegar en [preasivamos.ffie.com.co](https://preasivamos.ffie.com.co)

cuenta cambiar en proxy.conf.json
    "target":"https://preasivamosback.ffie.com.co",
    
luego utilice el comando
    npm run stag