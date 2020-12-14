import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-verdetalle-reclamacion-actuacion-cc',
  templateUrl: './verdetalle-reclamacion-actuacion-cc.component.html',
  styleUrls: ['./verdetalle-reclamacion-actuacion-cc.component.scss']
})
export class VerdetalleReclamacionActuacionCcComponent implements OnInit {
  idReclamacionActuacion: any;
  estadoAvanceReclamacion: any;
  fechaActuacionAdelantada: any;
  actuacionAdelantada: any;
  proximaActuacionRequerida: any;
  diasVencimientoTerminos: any;
  fechaVencimientoTerminos: any;
  observacionesAR: any;
  resultadoDefinitivo: any;
  urlSoporte: any;
  public codRecalamacion = localStorage.getItem('codReclamacion');
  public codReclamacionActuacion = localStorage.getItem('actuacionReclamacion');
  constructor(private services: ContractualControversyService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idReclamacionActuacion = param.id;
      this.loadData(this.idReclamacionActuacion);
    });
  }
  loadData(id) {
    this.services.GetActuacionSeguimientoById(id).subscribe((dataSeguimiento: any) => {
      this.estadoAvanceReclamacion = dataSeguimiento.estadoReclamacionCodigo;
      this.fechaActuacionAdelantada = dataSeguimiento.fechaActuacionAdelantada;
      this.actuacionAdelantada = dataSeguimiento.actuacionAdelantada;
      this.proximaActuacionRequerida = dataSeguimiento.proximaActuacion;
      this.diasVencimientoTerminos = dataSeguimiento.cantDiasVencimiento;
      this.fechaVencimientoTerminos = dataSeguimiento.fechaVencimiento;
      this.observacionesAR = dataSeguimiento.observaciones;
      switch (dataSeguimiento.esResultadoDefinitivo) {
        case false:
          this.resultadoDefinitivo = 'No';
          break;
        case true:
          this.resultadoDefinitivo = 'Sí';
          break;
      }
      this.urlSoporte = dataSeguimiento.rutaSoporte;
    });
  }
}
