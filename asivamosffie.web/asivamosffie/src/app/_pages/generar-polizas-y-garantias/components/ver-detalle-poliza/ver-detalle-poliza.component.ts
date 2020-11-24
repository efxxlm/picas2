import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';

@Component({
  selector: 'app-ver-detalle-poliza',
  templateUrl: './ver-detalle-poliza.component.html',
  styleUrls: ['./ver-detalle-poliza.component.scss']
})
export class VerDetallePolizaComponent implements OnInit {
  public arrayGarantias = [];
  polizasYSegurosArray: Dominio[] = [];
  estadoArray = [
    { name: 'Devuelta', value: '1' },
    { name: 'Aprobada', value: '2' }
  ];

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

  public obj1;
  public obj2;
  public obj3;
  public obj4;
  buenManejoCorrectaInversionAnticipo: any;
  estabilidadYCalidad: any;
  polizaYCoumplimiento: any;
  polizasYSegurosCompleto: any;

  cumpleDatosAsegurado: any;
  cumpleDatosBeneficiario: any;
  cumpleDatosTomador: any;
  incluyeReciboPago: any;
  incluyeCondicionesGenerales: any;

  fechaRevision: any;
  estadoRevision: any;
  fechaAprobacion: any;
  nomAprobado: any;
  observaciones: any;

  listaUsuarios: any[] = [];
  constructor(
    private polizaService: PolizaGarantiaService,
    private activatedRoute: ActivatedRoute,
    private common: CommonService,
    private contratacion:ProjectContractingService
  ) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.cargarDatosGenerales(param.id);
    });
  }
  cargarDatosGenerales(id) {
    this.polizaService.GetListVistaContratoGarantiaPoliza(id).subscribe(data => {
      this.fechaFirmaContrato = data[0].fechaFirmaContrato;
      this.tipoSolicitud = data[0].tipoSolicitud;
      this.numContrato = data[0].numeroContrato;
      this.tipoIdentificacion = data[0].tipoDocumento;
      if (data[0].objetoContrato != undefined || data[0].objetoContrato != null) {
        this.objeto = data[0].objetoContrato;
      }
      else {
        this.objeto = '';
      };
      this.tipoContrato = data[0].tipoContrato;
      if (data[0].valorContrato != undefined || data[0].valorContrato != null) {
        this.valorContrato = data[0].valorContrato;
      }
      else {
        this.valorContrato = 0;
      }
      this.plazoContrato = data[0].plazoContrato;
      this.loadContratacionId(data[0].contratacionId);
    });
    this.polizaService.GetContratoPolizaByIdContratoId(id).subscribe(data0 => {
      this.nomAseguradora = data0.nombreAseguradora;
      this.numPoliza = data0.numeroPoliza;
      this.numCertificado = data0.numeroCertificado;
      this.fechaExpPoliza = data0.fechaExpedicion;
      this.vigenciaPoliza = data0.vigencia;
      this.vigenciaAmparo = data0.vigenciaAmparo;
      this.valorAmparo = data0.valorAmparo;
      this.loadGarantia(data0.contratoPolizaId);
      this.loadLastObservation(data0.contratoPolizaId);
      this.loadChequeo(data0);
    });
    this.common.listaGarantiasPolizas().subscribe(data00 => {
      this.polizasYSegurosArray = data00;
    });
    this.common.getUsuariosByPerfil(10).subscribe(resp => {
      this.listaUsuarios = resp;
    });
    this.polizaService.GetNotificacionContratoPolizaByIdContratoId(id).subscribe(data01 => {
      this.fechaAprobacion = data01.fechaAprobacion;
    })
  }

  loadGarantia(polizaId) {
    this.polizaService.GetListPolizaGarantiaByContratoPolizaId(polizaId).subscribe(data1 => {
      const tipoGarantiaCodigo = [];
      this.arrayGarantias = data1;
      if (this.arrayGarantias.length > 0) {
        const polizasListRead = [this.arrayGarantias[0].tipoGarantiaCodigo];
        for (let i = 1; i < this.arrayGarantias.length; i++) {
          const Garantiaaux = polizasListRead.push(this.arrayGarantias[i].tipoGarantiaCodigo);
        }
        for (let i = 0; i < polizasListRead.length; i++) {
          const polizaSeleccionada = this.polizasYSegurosArray.filter(t => t.codigo === polizasListRead[i]);
          if (polizaSeleccionada.length > 0) { tipoGarantiaCodigo.push(polizaSeleccionada[0]) };
        }
        for (let j = 0; j < polizasListRead.length; j++) {
          switch (polizasListRead[j]) {
            case '1':
              this.obj1 = true;
              this.buenManejoCorrectaInversionAnticipo = this.arrayGarantias[j].esIncluidaPoliza;
              break;
            case '2':
              this.obj2 = true;
              this.estabilidadYCalidad = this.arrayGarantias[j].esIncluidaPoliza;
              break;
            case '3':
              this.obj3 = true;
              this.polizaYCoumplimiento = this.arrayGarantias[j].esIncluidaPoliza;
              break;
            case '4':
              this.obj4 = true;
              this.polizasYSegurosCompleto = this.arrayGarantias[j].esIncluidaPoliza;
              break;
          }
        }
      }
    });
  }
  loadLastObservation(polizaId){
    this.polizaService.GetListPolizaObservacionByContratoPolizaId(polizaId).subscribe(resp=>{
      this.polizaService.loadTableObservaciones.next(resp);
      for(let i=0; i<resp.length;i++){
        this.estadoRevision = resp[i].estadoRevisionCodigo;
        this.fechaRevision = resp[i].fechaRevision;
        this.observaciones = resp[i].observacion;
      }
    });
  }
  loadChequeo(data) {
    this.cumpleDatosAsegurado = data.cumpleDatosAsegurado;
    this.cumpleDatosBeneficiario = data.cumpleDatosBeneficiario;
    this.cumpleDatosTomador = data.cumpleDatosTomador;
    this.incluyeReciboPago = data.incluyeReciboPago;
    this.incluyeCondicionesGenerales = data.incluyeCondicionesGenerales;
    //Para el nombre aprobado de la granatia de la poliza
    for(let i=0; i<this.listaUsuarios.length;i++){
      if(this.listaUsuarios[i].usuarioId==parseInt(data.responsableAprobacion)){
        this.nomAprobado = this.listaUsuarios[i].nombres+" "+this.listaUsuarios[i].apellidos;
      }
    }
  }
  loadContratacionId(a){
    this.contratacion.getContratacionByContratacionId(a).subscribe(data=>{
      this.loadInfoContratacion(data);
    });
  }
  loadInfoContratacion(data) {
    this.nombreContratista = data.contratista.nombre;
    this.numeroIdentificacion = data.contratista.numeroIdentificacion;
  }
}
