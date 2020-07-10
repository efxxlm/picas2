import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registrar-nuevo',
  templateUrl: './registrar-nuevo.component.html',
  styleUrls: ['./registrar-nuevo.component.scss']
})
export class RegistrarNuevoComponent implements OnInit {

  tipoDeProceso: FormControl;

  procesos = [
    { name: 'Selección privada', value: 1 },
    { name: 'Invitación cerrada', value: 2 },
    { name: 'Invitación abierta', value: 3 }
  ];

  constructor(private router: Router,  ) {
    this.declararSelect();
  }

  ngOnInit(): void {
    this.tipoDeProceso.valueChanges
    .subscribe( (e) => {
      switch (e) {
        case 1:
          this.router.navigate(['/seleccion/seccionPrivada']);
          break;
        case 2:
          this.router.navigate(['/seleccion/invitacionCerrada']);
          break;
        case 3:
          this.router.navigate(['/seleccion/invitacionAbierta']);
          break;
      }
    });
  }

  private declararSelect() {
    this.tipoDeProceso = new FormControl('', [Validators.required]);
  }

}
