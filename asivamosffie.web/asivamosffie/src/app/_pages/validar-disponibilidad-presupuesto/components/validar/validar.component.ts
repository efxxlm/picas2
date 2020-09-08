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

  constructor(private disponibilidadServices: DisponibilidadPresupuestalService) {}

  ngOnInit(): void {
    this.disponibilidadServices.GetListGenerarDisponibilidadPresupuestal().subscribe(respuesta => 
      {
        this.listaDisponibilidades=respuesta;
        
        this.listaDisponibilidades.forEach(element => {
          //determino si esta completo o incompleto
          
          if(element.nombreEstado=='En validación presupuestal'||element.nombreEstado=='Devuelta por coordinación financiera')
          {            
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
         
        });
      }
    );
  }
}
