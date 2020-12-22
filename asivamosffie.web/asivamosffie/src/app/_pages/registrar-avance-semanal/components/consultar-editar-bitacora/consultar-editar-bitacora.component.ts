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
    this.avanceSemanalSvc.getListSeguimientoSemanalByContratacionProyectoId( this.activatedRoute.snapshot.params.id )
      .subscribe( bitacora => {
        this.consultarBitacora = bitacora;
        this.ultimaBitacora = this.consultarBitacora[ this.consultarBitacora.length - 1 ];
        console.log( this.ultimaBitacora );
      } );
  }

  ngOnInit(): void {
  }

}
