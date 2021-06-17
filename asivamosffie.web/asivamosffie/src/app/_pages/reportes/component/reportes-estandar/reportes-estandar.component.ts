import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ReportService } from 'src/app/core/_services/reportService/report.service';

@Component({
  selector: 'app-reportes-estandar',
  templateUrl: './reportes-estandar.component.html',
  styleUrls: ['./reportes-estandar.component.scss']
})
export class ReportesEstandarComponent implements OnInit {
  addressForm: FormGroup = this.fb.group({
    etapa: [null],
    procesoAcuerdo: [null],
    reporte: [null],
    departamento: [null],
    municipio: [null]
  });

  indicadorReporte: any[];
  reporteArray: any[];
  embedInfo: Object;

  etapaArray = [
    {
      name: 'Inicio',
      code: '1'
    },
    {
      name: 'Seguimiento',
      code: '2'
    },
    {
      name: 'Liquidación',
      code: '3'
    },
    {
      name: 'Comités',
      code: '4'
    }
  ];
  procesoAcuerdoArray = [
    {
      name: 'Registro de acuerdos de cofinanciación',
      code: '1'
    },
    {
      name: 'Registro de Proyectos',
      code: '2'
    },
    {
      name: 'Preparación',
      code: '3'
    },
    {
      name: 'Facturación',
      code: '4'
    },
    {
      name: 'Seguimiento técnico',
      code: '5'
    },
    {
      name: 'Seguimiento financiero',
      code: '6'
    },
    {
      name: 'Presupuestales',
      code: '7'
    },
    {
      name: 'Novedades',
      code: '8'
    },
    {
      name: 'Controversias',
      code: '9'
    },
    {
      name: 'Procesos de defensa judicial',
      code: '10'
    },
    {
      name: 'Entrega y liquidación',
      code: '11'
    },
    {
      name: 'Comité técnico',
      code: '12'
    },
    {
      name: 'Comité fiduciario',
      code: '13'
    }
  ];
  procesoAcuerdoInicio = [
    {
      name: 'Registro de acuerdos de cofinanciación',
      code: '1'
    },
    {
      name: 'Registro de Proyectos',
      code: '2'
    },
    {
      name: 'Preparación',
      code: '3'
    }
  ];
  procesoAcuerdoSeguimiento = [
    {
      name: 'Facturación',
      code: '4'
    },
    {
      name: 'Seguimiento técnico',
      code: '5'
    },
    {
      name: 'Seguimiento financiero',
      code: '6'
    },
    {
      name: 'Presupuestales',
      code: '7'
    },
    {
      name: 'Novedades',
      code: '8'
    },
    {
      name: 'Controversias',
      code: '9'
    },
    {
      name: 'Procesos de defensa judicial',
      code: '10'
    }
  ];
  procesoAcuerdoLiquidacion = [
    {
      name: 'Entrega y liquidación',
      code: '11'
    }
  ];
  procesoAcuerdoComites = [
    {
      name: 'Comité técnico',
      code: '12'
    },
    {
      name: 'Comité fiduciario',
      code: '13'
    }
  ];
  departamentoArray = [];
  municipioArray = [];
  constructor(private fb: FormBuilder, public commonService: CommonService, private report: ReportService) {}

  ngOnInit(): void {
    this.commonService.listaDepartamentos().subscribe(response => {
      this.departamentoArray = response;
    });

    this.getIndicadorReporte();
    this.filtrarIndicadores();
  }

  getIndicadorReporte() {
    this.report.getIndicadorReporte().subscribe(response => {
      this.indicadorReporte = response;
    });
  }

  filtrarIndicadores() {
    this.addressForm.get('etapa').valueChanges.subscribe(value => {
      switch (value) {
        case '1':
          this.procesoAcuerdoArray = this.procesoAcuerdoInicio;
          break;
        case '2':
          this.procesoAcuerdoArray = this.procesoAcuerdoSeguimiento;
          break;
        case '3':
          this.procesoAcuerdoArray = this.procesoAcuerdoLiquidacion;
          break;
        case '4':
          this.procesoAcuerdoArray = this.procesoAcuerdoComites;
          break;
        default:
          this.procesoAcuerdoArray = this.procesoAcuerdoArray;
          break;
      }
    });
    this.addressForm.valueChanges.subscribe(value => {
      this.reporteArray = this.indicadorReporte.filter(
        indicador => indicador.etapa === value.etapa && indicador.proceso === value.procesoAcuerdo
      );
    });
  }

  getMunicipio(event: MatSelectChange) {
    this.commonService.listaMunicipiosByIdDepartamento(event.value).subscribe(respuesta => {
      this.municipioArray = respuesta;
    });
  }
  onSubmit() {}

  getReportEmbedInfoByIndicadorReporteId(id: number) {
    this.report.getReportEmbedInfoByIndicadorReporteId(id).subscribe(response => {
      this.embedInfo = response;
    });
  }

  selectedOption(id: number) {
    this.getReportEmbedInfoByIndicadorReporteId(id);
  }
}
