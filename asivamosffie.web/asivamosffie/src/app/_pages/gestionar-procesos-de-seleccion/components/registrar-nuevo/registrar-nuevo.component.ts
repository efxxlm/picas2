import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ProcesoSeleccionService } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-registrar-nuevo',
  templateUrl: './registrar-nuevo.component.html',
  styleUrls: ['./registrar-nuevo.component.scss']
})
export class RegistrarNuevoComponent implements OnInit {

  tipoDeProceso: FormControl;

  procesos: Dominio[] = [];

  constructor(
              private router: Router,  
              private commonService: CommonService,
             ) 
  {
    this.declararSelect();
  }

  ngOnInit(): void {

    this.commonService.listaTipoProcesoSeleccion().subscribe( listPS => {
      this.procesos = listPS;
    }) 
    

    this.tipoDeProceso.valueChanges
    .subscribe( (e) => {
      switch (e.codigo) {
        case "1":
          this.router.navigate(['/seleccion/seccionPrivada']);
          break;
        case "2":
          this.router.navigate(['/seleccion/invitacionCerrada']);
          break;
        case "3":
          this.router.navigate(['/seleccion/invitacionAbierta']);
          break;
      }
    });
  }

  private declararSelect() {
    this.tipoDeProceso = new FormControl('', [Validators.required]);
  }

}
