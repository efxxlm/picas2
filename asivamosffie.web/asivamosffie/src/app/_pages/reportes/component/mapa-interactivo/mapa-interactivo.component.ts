import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ReportService } from 'src/app/core/_services/reportService/report.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-mapa-interactivo',
  templateUrl: './mapa-interactivo.component.html',
  styleUrls: ['./mapa-interactivo.component.scss']
})
export class MapaInteractivoComponent implements OnInit {
  addressForm = this.fb.group({
    quieroVer: [null, Validators.required]
  });

  indicadorReporte: any[];
  //estaEditando = false;
  constructor(private fb: FormBuilder, private report: ReportService, private router: Router) {}

  ngOnInit(): void {
    this.getIndicadorReporte();
  }

  getIndicadorReporte() {
    this.report.getIndicadorReporte().subscribe(response => {
      this.indicadorReporte = response.filter(indicador => indicador.indicador);
    });
  }

  showReport() {
    this.router.navigate(['/reportes/mapaInteractivo', this.addressForm.get('quieroVer').value]);
  }
}
