import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-ver-detalle-contrato-interventoria-artc',
  templateUrl: './ver-detalle-contrato-interventoria-artc.component.html',
  styleUrls: ['./ver-detalle-contrato-interventoria-artc.component.scss']
})
export class VerDetalleContratoInterventoriaArtcComponent implements OnInit {

  contrato: Contrato;
  perfilesCv: Dominio[] = [];

  constructor(
    private faseDosConstruccionSvc: FaseUnoConstruccionService,
    private commonSvc: CommonService,
    private activatedRoute: ActivatedRoute )
  {
    this.getContrato();
  }

  ngOnInit(): void {
  }

  getContrato() {
    this.commonSvc.listaPerfil()
      .subscribe(
        perfiles => {
          this.perfilesCv = perfiles;
          this.faseDosConstruccionSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
            .subscribe(
              response => {
                this.contrato = response;
              }
            );
        }
      );
  }

  getTipoPerfil( perfilCodigo: string ) {
    if ( this.perfilesCv.length > 0 ) {
      const tipoPerfil = this.perfilesCv.filter( value => value.codigo === perfilCodigo );
      return tipoPerfil[0].nombre;
    }
  }

}
