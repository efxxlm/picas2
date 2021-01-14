import { ActivatedRoute } from '@angular/router';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-consultar-editar-bitacora',
  templateUrl: './consultar-editar-bitacora.component.html',
  styleUrls: ['./consultar-editar-bitacora.component.scss']
})
export class ConsultarEditarBitacoraComponent implements OnInit {

  consultarBitacora: any;
  ultimaBitacora: any;

  constructor(
    private avanceSemanalSvc: RegistrarAvanceSemanalService,
    private activatedRoute: ActivatedRoute )
  {
    this.getBitacora();
  }

  ngOnInit(): void {
  }

  getBitacora() {
    this.consultarBitacora = undefined;
    this.ultimaBitacora = undefined;
    this.avanceSemanalSvc.getListSeguimientoSemanalByContratacionProyectoId( this.activatedRoute.snapshot.params.id )
        .subscribe( bitacora => {
            this.consultarBitacora = bitacora;
            console.log( this.consultarBitacora );
            this.ultimaBitacora = this.consultarBitacora[ this.consultarBitacora.length - 1 ];
        } );
  }

  valuePendingBitacora( value: boolean ) {
    if ( value === true ) {
      this.getBitacora();
    }
  }

}
