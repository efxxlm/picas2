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

  constructor(private disponibilidadServices: DisponibilidadPresupuestalService) {}

  ngOnInit(): void {
    this.disponibilidadServices.GetListGenerarDisponibilidadPresupuestal().subscribe(respuesta => 
      {
        this.listaDisponibilidades=respuesta;
        
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
