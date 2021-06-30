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
  listaDisponibilidadPresupuestalSum: any[] = [];

  constructor(private disponibilidadServices: DisponibilidadPresupuestalService) {}

  ngOnInit(): void {
    this.disponibilidadServices.GetListGenerarDisponibilidadPresupuestal(true).subscribe(respuesta =>
      {
        this.listaDisponibilidadPresupuestalSum = [];

        this.listaDisponibilidades=respuesta;

        this.listaDisponibilidades.forEach(element => {
          //rechazadas por al fiduciaria
          if(element.disponibilidadPresupuestal?.length > 0){
            element.disponibilidadPresupuestal.forEach(dp => {
              if(dp.rechazadaFiduciaria === true)
              {
                this.listaDisponibilidadPresupuestalSum.push(dp);
              }
            });
          }
          //determino si esta completo o incompleto
          if(element.nombreEstado=='Con validación presupuestal')
          {
            let cantcompleto=0;
            element.disponibilidadPresupuestal.forEach(element2 => {
              if(element2.estadoRegistro === true && element.numeroDdp === null)
              {
                cantcompleto++;
              }
            });
            console.log(cantcompleto, element.disponibilidadPresupuestal.length)
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
        //excepción - acordeón para los registros rechazadas por fiduciaria (contratación)
        console.log(this.listaDisponibilidadPresupuestalSum);
        this.listaDisponibilidades.push({
          disponibilidadPresupuestal: this.listaDisponibilidadPresupuestalSum,
          nombreEstado: "Rechazada por fiduciaria"
        });
      }
    );
  }

}
