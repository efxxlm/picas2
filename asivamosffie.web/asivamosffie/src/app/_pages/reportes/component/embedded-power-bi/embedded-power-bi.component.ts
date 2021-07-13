import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { ReportService } from 'src/app/core/_services/reportService/report.service';
import * as pbi from 'powerbi-client';

@Component({
  selector: 'app-embedded-power-bi',
  templateUrl: './embedded-power-bi.component.html',
  styleUrls: ['./embedded-power-bi.component.scss']
})
export class EmbeddedPowerBiComponent implements OnInit {
  embedId: number;
  embedInfo: Object;
  report: pbi.Embed;

  @ViewChild('reportContainer', { static: false }) reportContainer: ElementRef;

  constructor(private route: ActivatedRoute, private reportService: ReportService) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.embedId = params.id;
      this.getReportEmbedInfoByIndicadorReporteId(this.embedId);
    });
  }

  getReportEmbedInfoByIndicadorReporteId(id: number) {
    this.reportService.getReportEmbedInfoByIndicadorReporteId(id).subscribe(response => {
      this.embedInfo = response;
      this.showReport(this.embedInfo);
    });
  }

  showReport(token) {
    // Embed URL
    let embedUrl = token.embedParams.embedReport[0].embedUrl;
    let embedReportId = token.embedParams.embedReport.reportId;
    let config = {
      type: 'report',
      accessToken: token.embedParams.embedToken.token,
      embedUrl: embedUrl,
      id: embedReportId,
      tokenType: pbi.models.TokenType.Embed,
      settings: {}
    };
    let reportContainer = this.reportContainer.nativeElement;
    let powerbi = new pbi.service.Service(
      pbi.factories.hpmFactory,
      pbi.factories.wpmpFactory,
      pbi.factories.routerFactory
    );
    let report = powerbi.embed(reportContainer, config);
    report.off('loaded');
    report.on('loaded', () => {});
    report.on('error', () => {
      //this.getToken();
    });
  }
}
