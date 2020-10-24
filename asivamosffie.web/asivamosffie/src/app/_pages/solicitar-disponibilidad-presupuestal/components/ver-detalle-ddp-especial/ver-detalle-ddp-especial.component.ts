import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';

@Component({
  selector: 'app-ver-detalle-ddp-especial',
  templateUrl: './ver-detalle-ddp-especial.component.html',
  styleUrls: ['./ver-detalle-ddp-especial.component.scss']
})
export class VerDetalleDdpEspecialComponent implements OnInit {
  tipoSolicitudCodigo: string;
  objeto: string;
  numeroRadicadoSolicitud: string;
  cuentaCartaAutorizacion: boolean;
  cartaAutorizacionString: string;
  valorSolicitud: number;
  llaveMen: string;
  tipoIntervencion: number;
  nombreIE: string;
  sede: string;
  constructor(private activatedRoute: ActivatedRoute, private budgetAvailabilityService: BudgetAvailabilityService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.cargarRegistro( param.id );
    });
  }
  cargarRegistro( id: number )
  {
    this.budgetAvailabilityService.getDetailInfoAdditionalById( id )
      .subscribe( disponibilidad => {
        this.tipoSolicitudCodigo = disponibilidad.tipoSolicitudCodigo;
        this.objeto = disponibilidad.objeto;
        this.numeroRadicadoSolicitud = disponibilidad.numeroRadicadoSolicitud;
        this.cuentaCartaAutorizacion = disponibilidad.cuentaCartaAutorizacion;
        if(this.cuentaCartaAutorizacion==true){
          this.cartaAutorizacionString="Sí";
        }
        else{
          this.cartaAutorizacionString="No";
        }
        this.valorSolicitud = disponibilidad.valorSolicitud;
        this.llaveMen = disponibilidad.disponibilidadPresupuestalProyecto[0].proyecto.llaveMen;
        this.budgetAvailabilityService.searchLlaveMEN(this.llaveMen).subscribe(data=>{
          this.tipoIntervencion= data[0].tipoIntervencionCodigo;
          this.nombreIE= data[0].institucionEducativa.nombre;
          this.sede= data[0].sede.nombre;
        });
      })
  }
}
