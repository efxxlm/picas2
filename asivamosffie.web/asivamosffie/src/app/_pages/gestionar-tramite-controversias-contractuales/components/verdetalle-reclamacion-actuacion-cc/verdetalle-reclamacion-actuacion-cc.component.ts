import { Component, OnInit } from '@angular/core';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-verdetalle-reclamacion-actuacion-cc',
  templateUrl: './verdetalle-reclamacion-actuacion-cc.component.html',
  styleUrls: ['./verdetalle-reclamacion-actuacion-cc.component.scss']
})
export class VerdetalleReclamacionActuacionCcComponent implements OnInit {

  constructor(private services: ContractualControversyService) { }

  ngOnInit(): void {
  }

}
