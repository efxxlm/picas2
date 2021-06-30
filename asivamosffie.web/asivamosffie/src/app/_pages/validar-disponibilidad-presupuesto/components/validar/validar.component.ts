import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';

@Component({
  selector: 'app-validar',
  templateUrl: './validar.component.html',
  styleUrls: ['./validar.component.scss']
})
export class ValidarComponent implements OnInit {

  verAyuda = false;
  listaDisponibilidades: any;
  listaestado=['En validación presupuestal','Devuelta por coordinación financiera',
    'Devuelta por validación presupuestal','Con validación presupuestal',
    'Con disponibilidad presupuestal','Rechazada por validación presupuestal','Con disponibilidad cancelada']

  devueltaPresupuestal_class:string;

  constructor(private disponibilidadServices: DisponibilidadPresupuestalService) {}

  validarCompletos( disponibilidad: any ){
    disponibilidad['class'] = 2;// completo

    //disponibilidad['class'] = 0; // sin diligenciar

    // if (disponibilidad.disponibilidadPresupuestal.length === 0)
    //   disponibilidad['class'] = 2;// completo

    disponibilidad.disponibilidadPresupuestal.forEach(d => {
      if (d.estadoRegistro !== true )
        //disponibilidad['class'] = 1;// incompleto
        disponibilidad['class'] = 0; // sin diligenciar
    });
  }

  pintarSemaforos(){
    this.listaDisponibilidades.forEach(disponibilidad => {
      switch (disponibilidad.nombreEstado){
        case 'En validación presupuestal':
          this.validarCompletos( disponibilidad );
        break;

        case 'Devuelta por validación presupuestal':
          this.validarCompletos( disponibilidad );
        break;

        case 'Devuelta por coordinación financiera':
          this.validarCompletos( disponibilidad );
        break;

        case 'Con validación presupuestal':
          this.validarCompletos( disponibilidad );
          break;

        case 'Con disponibilidad presupuestal':
          this.validarCompletos( disponibilidad );
          break;

        case 'Rechazada por validación presupuestal':
          this.validarCompletos( disponibilidad );
        break;

        case 'Con disponibilidad cancelada':
          this.validarCompletos( disponibilidad );
        break;
      }
    });
  }

  ngOnInit(): void {
    this.disponibilidadServices.GetListGenerarDisponibilidadPresupuestal(false).subscribe(respuesta =>
      {
        this.listaDisponibilidades=respuesta;

        this.pintarSemaforos();

        this.listaDisponibilidades.forEach(element => {
          //determino si esta completo o incompleto

          if(element.nombreEstado=='En validación presupuestal'||element.nombreEstado=='Devuelta por coordinación financiera')
          {
            console.log(element.disponibilidadPresupuestal.length);
            if(element.disponibilidadPresupuestal.length==0)
            {
              element.completo="";
            }
            else{
              let cantcompleto=0;
              element.disponibilidadPresupuestal.forEach(element2 => {
                if(element2.estadoRegistro)
                {
                  cantcompleto++;
                }
              });
              if(cantcompleto==element.disponibilidadPresupuestal.length)
              {
                element.completo='Completo';
              }
              else{
                if(element.disponibilidadPresupuestal.length>0)
                {
                  element.completo='Incompleto';
                }
                else{
                  element.completo='';
                }
              }
            }
          }
        });
      }
    );
  }
}
