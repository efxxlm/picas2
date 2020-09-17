import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss']
})
export class TituloComponent implements OnInit {

  tipoDisponibilidad: FormControl;

  selectDisponibilidad: Dominio[] = [];
  perfil: number = 0;
    
  constructor(
              private router: Router,
              private commonService: CommonService
             ) 
  { }

  ngOnInit(): void {

    let session =  JSON.parse(localStorage.getItem('actualUser'));
    if ( session ){
      session.rol.forEach(element => {
        this.perfil = element.perfilId;
      });
    }

    if ( this.perfil == 5 ){

    }

    this.declararSelect();
    
    this.commonService.listaTipoDisponibilidadPresupuestal().subscribe( respuesta =>  {
      this.selectDisponibilidad = respuesta;
    })

  }

  private declararSelect() {
    this.tipoDisponibilidad = new FormControl('', [Validators.required]);
  }

  crearSolicitud() {


    if ( this.perfil == 5 ){
      this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudAdministrativa/nueva',0])
    }else {
      switch (this.tipoDisponibilidad.value.codigo) {
        case "1":
          this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudTradicional']);
          break;
        case "2":
          this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudEspecial']);
          break;
      }
    }
  }
}
