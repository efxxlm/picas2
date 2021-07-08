import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ReportService } from 'src/app/core/_services/reportService/report.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reportes-estandar',
  templateUrl: './reportes-estandar.component.html',
  styleUrls: ['./reportes-estandar.component.scss']
})
export class ReportesEstandarComponent implements OnInit {
  addressForm = this.fb.group({
    etapa: [null, Validators.required],
    procesoAcuerdo: [null, Validators.required],
    reporte: [null, Validators.required]
  });

  indicadorReporte: any[];
  reporteArray: any[];

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

  constructor(private fb: FormBuilder, private report: ReportService, private router: Router) {}

  ngOnInit(): void {
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

  showReport() {
    this.router.navigate(['/reportes/reportesEstandar', this.addressForm.get('reporte').value]);
  }
}
