import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';

@Component({
  selector: 'app-ver-detalle-poliza',
  templateUrl: './ver-detalle-poliza.component.html',
  styleUrls: ['./ver-detalle-poliza.component.scss']
})
export class VerDetallePolizaComponent implements OnInit {
  fechaFirmaContrato: any;
  tipoSolicitud: any;
  tipoContrato: any;
  objeto: any;
  nombreContratista: any;
  tipoIdentificacion: any;
  numeroIdentificacion: any;
  valorContrato: any;
  plazoContrato: any;
  numContrato: any;
  nomAseguradora: any;
  numPoliza: any;
  numCertificado: any;
  fechaExpPoliza: any;
  vigenciaPoliza: any;
  vigenciaAmparo: any;
  valorAmparo: number;

  constructor(    
    private polizaService: PolizaGarantiaService,
    private activatedRoute: ActivatedRoute,
    ) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe( param => {
      this.cargarDatosGenerales(param.id);
    });
  }
  cargarDatosGenerales(id){
    this.polizaService.GetListVistaContratoGarantiaPoliza(id).subscribe(data => {
      this.fechaFirmaContrato = data[0].fechaFirmaContrato;
      this.tipoSolicitud = data[0].tipoSolicitud;
      this.tipoContrato = data[0].tipoContrato;
      this.objeto = data[0].descripcionModificacion;
      this.nombreContratista = data[0].nombreContratista;
      this.tipoIdentificacion = data[0].tipoDocumento;
      this.numeroIdentificacion = data[0].numeroIdentificacion;
      this.valorContrato = data[0].valorContrato;
      this.plazoContrato = data[0].plazoContrato;
      this.numContrato = data[0].numeroContrato;
    });
    this.polizaService.GetContratoPolizaByIdContratoId(id).subscribe(data0=>{
      this.nomAseguradora = data0.nombreAseguradora;
      this.numPoliza = data0.numeroPoliza;
      this.numCertificado = data0.numeroCertificado;
      this.fechaExpPoliza = data0.fechaExpedicion;
      this.vigenciaPoliza = data0.vigencia;
      this.vigenciaAmparo = data0.vigenciaAmparo;
      this.valorAmparo = data0.valorAmparo;
    });
  }
}
