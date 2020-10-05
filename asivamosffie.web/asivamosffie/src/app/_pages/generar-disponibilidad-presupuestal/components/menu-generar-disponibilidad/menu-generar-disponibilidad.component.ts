import { Component, OnInit } from '@angular/core';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';

@Component({
  selector: 'app-menu-generar-disponibilidad',
  templateUrl: './menu-generar-disponibilidad.component.html',
  styleUrls: ['./menu-generar-disponibilidad.component.scss']
})
export class MenuGenerarDisponibilidadComponent implements OnInit {

  verAyuda = false;
  listaDisponibilidades: any[]=[];

  constructor(private disponibilidadServices: DisponibilidadPresupuestalService) {}

  ngOnInit(): void {
    this.disponibilidadServices.GetListGenerarDisponibilidadPresupuestal().subscribe(respuesta => 
      {
        this.listaDisponibilidades=respuesta;
        
        this.listaDisponibilidades.forEach(element => {
          //determino si esta completo o incompleto
          
          if(element.nombreEstado=='Con validación presupuestal')
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
