import { Component, Input, OnInit } from '@angular/core';
import { ReleaseBalanceService } from 'src/app/core/_services/releaseBalance/release-balance.service';

@Component({
  selector: 'app-liberacion-saldo',
  templateUrl: './liberacion-saldo.component.html',
  styleUrls: ['./liberacion-saldo.component.scss']
})
export class LiberacionSaldoComponent implements OnInit {

  @Input() id: number;
  @Input() esVerDetalle: boolean;
  estadoInforme = 0;
  drps: any;

  constructor(
    private releaseBalanceService: ReleaseBalanceService,
  ) { }

  ngOnInit(): void {
    this.getDrpByProyectoId();
  }

  getDrpByProyectoId() {
    this.releaseBalanceService.getDrpByProyectoId(this.id).subscribe(data => {
        this.drps = data;
    });
  }
}

