import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { ReportService } from 'src/app/core/_services/reportService/report.service';

@Component({
  selector: 'app-mapa-interactivo',
  templateUrl: './mapa-interactivo.component.html',
  styleUrls: ['./mapa-interactivo.component.scss']
})
export class MapaInteractivoComponent implements OnInit {
  addressForm = this.fb.group({
    quieroVer: [null]
  });
  myFilter = new FormControl();
  indicadorReporte: any[];
  embedInfo: Object;
  //estaEditando = false;
  constructor(private fb: FormBuilder, private report: ReportService) {}

  ngOnInit(): void {
    this.getIndicadorReporte();
  }

  getIndicadorReporte() {
    this.report.getIndicadorReporte().subscribe(response => {
      this.indicadorReporte = response;
    });
  }

  getReportEmbedInfo() {
    this.report.getReportEmbedInfo().subscribe(response => {
      console.log(response);
    });
  }

  getReportEmbedInfoByIndicadorReporteId(id: number) {
    this.report.getReportEmbedInfoByIndicadorReporteId(id).subscribe(response => {
      this.embedInfo = response;
    });
  }

  selectedOption(id: number) {
    this.getReportEmbedInfoByIndicadorReporteId(id);
  }
}
