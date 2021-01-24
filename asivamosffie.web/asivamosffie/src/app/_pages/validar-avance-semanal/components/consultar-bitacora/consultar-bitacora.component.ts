import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';

@Component({
  selector: 'app-consultar-bitacora',
  templateUrl: './consultar-bitacora.component.html',
  styleUrls: ['./consultar-bitacora.component.scss']
})
export class ConsultarBitacoraComponent implements OnInit {

    consultarBitacora: any;
    ultimaBitacora: any;

    constructor(
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private activatedRoute: ActivatedRoute )
    { }

    ngOnInit(): void {
        this.getBitacora();
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

}
