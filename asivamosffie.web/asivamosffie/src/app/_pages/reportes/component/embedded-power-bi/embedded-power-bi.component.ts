import {
  Component,
  ElementRef,
  OnInit,
  AfterViewInit,
  OnChanges,
  SimpleChanges,
  ViewChild,
  Input
} from '@angular/core';
import * as pbi from 'powerbi-client';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-embedded-power-bi',
  templateUrl: './embedded-power-bi.component.html',
  styleUrls: ['./embedded-power-bi.component.scss']
})
export class EmbeddedPowerBiComponent implements OnInit, AfterViewInit, OnChanges {
  @Input() embedInfo: any;

  report: pbi.Embed;
  @ViewChild('reportContainer', { static: false }) reportContainer: ElementRef;
  constructor() {}

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.showReport(this.embedInfo);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.embedInfo) {
      this.ngAfterViewInit();
    }
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
